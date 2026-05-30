As pastas ./build e ./deploy são necessárias para rodar a esteira no jenkins de build e deploy do projeto.

As pastas fargate e swarm, fazem parte do script de automatização de criação de solution, em que dependendo do tipo de projeto, ele irá copiar os scripts de build e deploy, e então, colar dentro da pasta ./build e ./deploy para substituir os arquivos já existentes.