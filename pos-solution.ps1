. .\script-format-json.ps1

Write-Output "Removendo projetos de teste n�o utilizados."
dotnet sln Richter.WhoAmIApi.sln remove tests/Richter.WhoAmIApi.Application.Tests/Richter.WhoAmIApi.Application.Tests.csproj
dotnet sln Richter.WhoAmIApi.sln remove tests/Richter.WhoAmIApi.CrossCutting.Tests/Richter.WhoAmIApi.CrossCutting.Tests.csproj

Write-Output "Removendo pacotes n�o utilizados."
Push-Location src/Autoglass.Atendimentos.Application
dotnet remove package Autoglass.Libs.Comum
Pop-Location

Write-Output "Removendo projeto de Workers."
dotnet sln Richter.WhoAmIApi.sln remove src/Richter.WhoAmIApi.Workers/Richter.WhoAmIApi.Workers.csproj

Write-Output "Removendo projeto de Consumers."
dotnet sln Richter.WhoAmIApi.sln remove src/Richter.WhoAmIApi.Consumers/Richter.WhoAmIApi.Consumers.csproj

Write-Output "Removendo projeto de Jobs."
dotnet sln Richter.WhoAmIApi.sln remove src/Richter.WhoAmIApi.Jobs/Richter.WhoAmIApi.Jobs.csproj

Write-Output "Removendo configura��es do Oracle."

Push-Location src/Autoglass.Atendimentos.Domain
dotnet remove package Autoglass.Libs.NHibernate
Pop-Location

Push-Location src/Autoglass.Atendimentos.Infra
dotnet remove package FluentNHibernate
Pop-Location

Push-Location src/Autoglass.Atendimentos.IoC
dotnet remove package Autoglass.Libs.Seguranca
Pop-Location

Write-Output "Removendo configura��es do Sqs."

Push-Location src/Autoglass.Atendimentos.Domain
dotnet remove package Autoglass.Libs.Aws.Sqs
Pop-Location

Write-Output "Removendo configura��es do Kafka."

Push-Location src/Autoglass.Atendimentos.Domain
dotnet remove package Autoglass.Libs.Core.EventStream
Pop-Location

Write-Output "Removendo configura��es do Mensageria."

Push-Location src/Autoglass.Atendimentos.Infra
dotnet remove package Autoglass.Libs.Mensageria
Pop-Location

Write-Output "Removendo configura��es de Feature Flags."

Push-Location src/Autoglass.Atendimentos.IoC
dotnet remove package Autoglass.Libs.Aws.FeatureManager
Pop-Location

# Formata arqwuivos .json para remover v�rgulas incorretas.
FormatJson -Path "src/Autoglass.Atendimentos.IoC/appsettings.json"
FormatJson -Path "src/Autoglass.Atendimentos.IoC/appsettings.Development.json"
FormatJson -Path "src/Autoglass.Atendimentos.IoC/appsettings.Hml.json"
FormatJson -Path "src/Autoglass.Atendimentos.IoC/appsettings.Prod.json"

Write-Output "Criando pasta 'Controllers' no projeto de API."
New-Item -ItemType Directory -Path "src/Autoglass.Atendimentos.API/Controllers"




# Exclui o script ap�s a execu��o.
$scriptPath = $MyInvocation.MyCommand.Path
Remove-Item -Path $scriptPath -Force