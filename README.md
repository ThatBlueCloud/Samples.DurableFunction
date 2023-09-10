# Sample Durable Function
This is a sample code to be used in the That Blue Cloud blog posts, based on Azure Functions' Durable Function template. Its purpose is to demonstrate connectivity to Durable Functions from other platforms, such as Microsoft Fabric.

It accepts a JSON payload with a single property called `name`, which then runs in the background with a random delay of 45-300 seconds.

Visit That Blue Cloud [https://thatbluecloud.com](https://thatbluecloud.com) and subscribe to be notified of any new articles on Microsoft Fabric, Synapse Analytics and other Azure resources.

## Request Body Sample
```json
{
    "name": "That Blue Cloud"
}
```