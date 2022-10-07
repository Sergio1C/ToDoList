$EXEPATH = "F:\Sources\ToDoList\ToDo.Service\bin\Debug\net6.0"
$EXEFILEPATH = "$EXEPATH\ToDo.exe";
$SERVICENAME = "ToDoService";

<#Create service user account #>
New-LocalUser -Name $SERVICENAME

$acl = Get-Acl $EXEPATH
$aclRuleArgs = "$env:computername\$SERVICENAME", "Read,Write,ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($aclRuleArgs)
$acl.SetAccessRule($accessRule)
$acl | Set-Acl $EXEPATH

New-Service -Name $SERVICENAME -BinaryPathName "$EXEFILEPATH --contentRoot $EXEPATH" -Credential ".\$SERVICENAME" -Description "A service required by MOAB company to job apply." -DisplayName "ToDo list service" -StartupType Automatic

Start-Service -Name $SERVICENAME


<# 
https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-6.0&tabs=visual-studio#log-on-as-a-service-rights-1
#>