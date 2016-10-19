$ServicePlanName = "DemoPlan"
$WebAppName = "DemoApp"
$ResourceGroupName = "SampleRG"
$WebAppLocation = "northeurope"
$ErrorActionPreference = "Stop"

# Step 1: Create the application service plan
Create-AppServicePlan

# Step 2: Create the web app using the service plan name.
Create-AzureRmWebApp

function Create-AzureRmWebApp()
{
    #https://msdn.microsoft.com/en-us/library/mt619250.aspx
    $resource = Find-AzureRmResource -ResourceNameContains $WebAppName -ResourceGroupNameContains $ResourceGroupName -ResourceType "Microsoft.Web/sites"
    if(!$resource)
    {
        $webApp = New-AzureRmWebApp -ResourceGroupName $ResourceGroupName -Name $WebAppName -Location $WebAppLocation -AppServicePlan $ServicePlanName
    }
}

function Create-AppServicePlan()
{
    #https://msdn.microsoft.com/en-us/library/mt619306.aspx
    $resource = Find-AzureRmResource -ResourceNameContains $ServicePlanName -ResourceGroupNameContains $ResourceGroupName -ResourceType "Microsoft.Web/ServerFarms"
    if(!$resource)
    {
        # Specify the Tier type that you would like
        $servicePlan = New-AzureRmAppServicePlan -ResourceGroupName $ResourceGroupName -Name $ServicePlanName -Location $WebAppLocation -Tier Standard -NumberofWorkers 1 -WorkerSize "Small"
    }
}