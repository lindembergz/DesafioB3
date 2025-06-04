# CDB Aplicação

Esta solução implementa uma calculadora de investimentos em CDB (Certificado de Depósito Bancário), composta por uma API em .NET e uma interface web em Angular.


![image](https://github.com/user-attachments/assets/5e1629e9-27cc-42b7-941a-53bcd5d34370)



## Estrutura da Solução

A solução está organizada em uma arquitetura em camadas:

- **CDB.API**: Projeto Web API que expõe endpoints para cálculo de investimentos
- **CDB.Application**: Camada de aplicação com serviços, DTOs e validações
- **CDB.Domain**: Camada de domínio com modelos e interfaces
- **CDB.Infrastructure**: Configuração de injeção de dependências
- **CDB.Tests**: Testes unitários
- **CDB.Web**: Interface de usuário em Angular

## Requisitos

- .NET 8.0 ou superior
- Node.js 16.x ou superior
- Angular CLI 15.x ou superior
- Visual Studio 2022 

## Configuração e Execução

### Backend (API)

1. Abra a solução `CDBApplication.sln` no Visual Studio
2. A solução baixará os pacotes automaticamente, caso contrário restaure os pacotes NuGet (clique com o botão direito na solução → Restaurar Pacotes NuGet)
3. Expanda a pasta BackEnd e Compile o projeto CDB.API
4. Selecione e Execute o projeto CDB.API 
   - A API será iniciada em `http://localhost:5189` (a porta pode variar)
   - O Swagger estará disponível 

### Frontend (Angular)

1. Navegue até a pasta do projeto web no terminal: `cd ....\CDBFrontEnd\BBBInvestimento`
2. Instale as dependências: `npm install`
3. Na classe de serviço "InvestimentoService" estará apontando para a API
  
4. Execute a aplicação: `ng serve` ou `npm start`
5. Acesse a aplicação em `http://localhost:4200`

## Utilizando a Aplicação

1. Na interface web, informe:
   - **Valor Inicial**: Valor monetário positivo (de R$ 0,01 até R$ 1.000.000.000,00)
   - **Prazo**: Número de meses (de 2 até 240 meses)
2. Clique no botão "Calcular"
3. Os resultados exibidos serão:
   - **Valor Bruto**: Valor final do investimento antes dos impostos
   - **Valor Líquido**: Valor final após descontar o imposto sobre o rendimento

## Executando os Testes

### Testes da API (.NET)

1. No Visual Studio, abra o Test Explorer (Test → Test Explorer)
2. Clique em Run All para executar todos os testes unitários
3. Para verificar a cobertura de código, utilize a ferramenta de cobertura integrada:
   - Instale a extensão "Fine Code Coverage"
   - Estará disponível no menu Exibir -> Outras Janelas -> Fine Code Coverage
   - Execute os testes para observar as métricas

## Notas Técnicas

- A API implementa validações para garantir dados de entrada válidos
- Os valores são calculados com precisão decimal para evitar erros de arredondamento
- O cálculo é aplicado mês a mês, utilizando o rendimento acumulado
- O imposto é aplicado apenas sobre o rendimento (diferença entre valor final e inicial)

## AWS Deployment with GitHub Actions

This project includes a GitHub Actions workflow (`.github/workflows/aws-deploy.yml`) to automate deployment to AWS. To use this workflow, you need to configure AWS credentials as GitHub secrets and customize the deployment steps.

### 1. Configure AWS Credentials as GitHub Secrets

You must store your AWS credentials securely as GitHub secrets. Do **not** hardcode them in the workflow file or any other part of the repository.

- **AWS_ACCESS_KEY_ID**: Your AWS access key ID.
- **AWS_SECRET_ACCESS_KEY**: Your AWS secret access key.

To add these secrets:
1. Go to your GitHub repository.
2. Click on **Settings**.
3. In the left sidebar, navigate to **Secrets and variables** > **Actions**.
4. Click on **New repository secret**.
5. Add `AWS_ACCESS_KEY_ID` with your access key ID as the value.
6. Add `AWS_SECRET_ACCESS_KEY` with your secret access key as the value.

The workflow is configured to use `us-east-1` as the AWS region. You can change this in the `.github/workflows/aws-deploy.yml` file if needed.

### 2. Customize Deployment Steps

The workflow file (`.github/workflows/aws-deploy.yml`) contains placeholder steps for deployment. You **must** replace these placeholders with the actual commands required to deploy your application to your specific AWS services (e.g., S3, EC2, ECS, Elastic Beanstalk, Lambda, etc.).

Open `.github/workflows/aws-deploy.yml` and look for the section commented with:

```yaml
      # Add your deployment steps here.
      # For example, if deploying a static site to S3:
      # - name: Deploy static site to S3
      #   run: aws s3 sync ./CDBFrontEnd/BBBInvestimento/dist/your-s3-bucket-name/ --delete
      #
      # Or if deploying a .NET application to Elastic Beanstalk:
      # ... (examples for .NET deployment)

      - name: Placeholder for AWS Deployment
        run: |
          echo "AWS deployment steps go here."
          echo "Please replace this placeholder with your actual deployment commands."
          # ...
```

Update this section with the commands appropriate for your AWS setup.
