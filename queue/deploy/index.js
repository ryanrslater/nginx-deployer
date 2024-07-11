import amqp from 'amqplib'
import AWS from 'aws-sdk'

const connectionstring = 'amqp://localhost'

const queueName = 'deploy'

const connect = async () => {
    const connection = await amqp.connect(connectionstring)
    const channel = await connection.createChannel()
    await channel.assertQueue(queueName)
    return channel
}

const consumer = async () => {
    const channel = await connect()
    channel.prefetch(1)
    channel.consume(queueName, async (msg) => {
        console.log('Received:', msg.content.toString())
        AWS.config.update({ region: 'us-east-1' })
        var ec2 = new AWS.EC2({ apiVersion: '2016-11-15' })
        var instanceParams = {
            ImageId: 'AMI_ID',
            InstanceType: 't2.micro',
            KeyName: 'KEY_PAIR_NAME',
            MinCount: 1,
            MaxCount: 1,
        }

        const instancePromise = await ec2.runInstances(instanceParams).promise()

        const instanceId = instancePromise.Instances[0].InstanceId

        console.log('Created instance', instanceId)

        const tagParams = {
            Resources: [instanceId],
            Tags: [
                {
                    Key: 'Name',
                    Value: 'SDK Sample',
                },
            ],
        }

        const tagPromise = await ec2.createTags(tagParams).promise()

        channel.ack(msg)
    })
}

consumer()
