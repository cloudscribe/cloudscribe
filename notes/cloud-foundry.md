# cloud foundry

https://docs.cloudfoundry.org/concepts/overview.html

How the Cloud Balances Its Load
Clouds balance their processing loads over multiple machines, optimizing for efficiency and resilience against point failure. A Cloud Foundry installation accomplishes this at three levels:
BOSH creates and deploys virtual machines (VMs) on top of a physical computing infrastructure, and deploys and runs Cloud Foundry on top of this cloud. To configure the deployment, BOSH follows a manifest document.
The CF Cloud Controller runs the apps and other processes on the cloud’s VMs, balancing demand and managing app lifecycles.
The router routes incoming traffic from the world to the VMs that are running the apps that the traffic demands, usually working with a customer-provided load balancer.

https://docs.cloudfoundry.org/cf-cli/

## Buildpacks

Buildpacks provide needed dependencies to Cloud Foundry apps
https://docs.cloudfoundry.org/buildpacks/dotnet-core/index.html

https://github.com/IBM-Cloud/aspnet-core-helloworld


## on azure

https://docs.microsoft.com/en-us/azure/cloudfoundry/

https://pivotal.io/partners/microsoft-azure

There are two forms of Cloud Foundry available to run on Azure: open-source Cloud Foundry (OSS CF) and Pivotal Cloud Foundry (PCF). OSS CF is an entirely open-source version of Cloud Foundry managed by the Cloud Foundry Foundation. Pivotal Cloud Foundry is an enterprise distribution of Cloud Foundry from Pivotal Software Inc.

Push applications to any Cloud Foundry instance from VS
https://marketplace.visualstudio.com/items?itemName=ms-vsts.cloud-foundry-build-extension