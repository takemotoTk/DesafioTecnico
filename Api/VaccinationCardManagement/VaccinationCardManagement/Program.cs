using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.Intrinsics.X86;
using System.Text;
using VaccinationCardManagement.Application;
using VaccinationCardManagement.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Custom Swagger
var titleApi = "Gest�o do Cart�o de Vacina��o";
var decriptionHelper = "<b>Funcionalidades:</b>\r\n\r\n";
decriptionHelper += "<b>Authentication:</b>\r\n\r\n";
decriptionHelper += "Gera token de acesso: Existe um pr� cadastro do usu�rio 'brunotakemoto@gmail.com' e senha '123456', para gerar 1 token de acesso que pode ser informado no 'Authorize'(localizado no topo da p�gina na cor verde).\r\n\r\n";
decriptionHelper += "Criar um usu�rio: pode ser  criado 1 usu�rio para acesso ao sistema.\r\n\r\n";
decriptionHelper += "Consultar Usu�rio Logado: Ap�s gerar o token e informado no 'Authorize'(localizado no topo da p�gina na cor verde), voc� pode consultar o nome do usu�rio logado.\r\n\r\n\r\n\r\n";

decriptionHelper += "<b>CacheManager:</b>\r\n\r\n";
decriptionHelper += "Consultar todas as chaves que possuem dados no cache: Algumas funcionalidades fazem o cache de mem�ria para n�o precisarem ir ao banco toda vez, voc� pode consultar quais chaves est�o salvas no cache.\r\n\r\n";
decriptionHelper += "Deletar o cache pela chave: Voc� pode apagar 1 chave do cache, implicando assim em apagar os dados dessa chave.\r\n\r\n";
decriptionHelper += "Limpar todo o cache: Esse cara vai limpar todo cache criado.\r\n\r\n\r\n\r\n";

decriptionHelper += "<b>Vaccine:</b>\r\n\r\n";
decriptionHelper += "Cadastrar uma vacina: Uma vacina consiste em um nome e um identificador �nico.\r\n\r\n\r\n\r\n";

decriptionHelper += "<b>Person:</b>\r\n\r\n";
decriptionHelper += "Cadastrar uma pessoa: Uma pessoa consiste em um nome e um n�mero de identifica��o �nico.\r\n\r\n";
decriptionHelper += "Remover uma pessoa: Uma pessoa pode ser removida do sistema, o que tamb�m implica na exclus�o de seu cart�o de vacina��o e todos os registros associados.\r\n\r\n\r\n\r\n";

decriptionHelper += "<b>Cart�o de Vacina��o:</b>\r\n\r\n";
decriptionHelper += "Cadastrar uma vacina��o: Para uma pessoa cadastrada, � poss�vel registrar uma vacina��o, fornecendo informa��es como o identificador da vacina e a dose aplicada (A dose deve ser validada pelo sistema).\r\n\r\n";
decriptionHelper += "Consultar o cart�o de vacina��o de uma pessoa: Permite visualizar todas as vacinas registradas no cart�o de vacina��o de uma pessoa, incluindo detalhes como o nome da vacina, data de aplica��o e doses recebidas.\r\n\r\n";
decriptionHelper += "Excluir registro de vacina��o: Permite excluir um registro de vacina��o espec�fico do cart�o de vacina��o de uma pessoa.\r\n\r\n\r\n\r\n";

builder.Services.AddSwaggerService(builder.Configuration, titleApi, 1, decriptionHelper, true);

//Add Application Dependency Module
builder.Services.AddApplicationModuleDependency(builder.Configuration);

//Authentication JWT
builder.Services.ConfigureSwaggerSecurityService(builder.Configuration);
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]))
        };
    });
builder.Services.AddAuthorization();

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//CORS
app.UseCors("AllowAll");

//Configure Screen
app.ConfigureSwaggerApplication(builder.Environment, true);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
