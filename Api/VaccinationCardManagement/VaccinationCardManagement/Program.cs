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
var titleApi = "Gestão do Cartão de Vacinação";
var decriptionHelper = "<b>Funcionalidades:</b>\r\n\r\n";
decriptionHelper += "<b>Authentication:</b>\r\n\r\n";
decriptionHelper += "Gera token de acesso: Existe um pré cadastro do usuário 'brunotakemoto@gmail.com' e senha '123456', para gerar 1 token de acesso que pode ser informado no 'Authorize'(localizado no topo da página na cor verde).\r\n\r\n";
decriptionHelper += "Criar um usuário: pode ser  criado 1 usuário para acesso ao sistema.\r\n\r\n";
decriptionHelper += "Consultar Usuário Logado: Após gerar o token e informado no 'Authorize'(localizado no topo da página na cor verde), você pode consultar o nome do usuário logado.\r\n\r\n\r\n\r\n";

decriptionHelper += "<b>CacheManager:</b>\r\n\r\n";
decriptionHelper += "Consultar todas as chaves que possuem dados no cache: Algumas funcionalidades fazem o cache de memória para não precisarem ir ao banco toda vez, você pode consultar quais chaves estão salvas no cache.\r\n\r\n";
decriptionHelper += "Deletar o cache pela chave: Você pode apagar 1 chave do cache, implicando assim em apagar os dados dessa chave.\r\n\r\n";
decriptionHelper += "Limpar todo o cache: Esse cara vai limpar todo cache criado.\r\n\r\n\r\n\r\n";

decriptionHelper += "<b>Vaccine:</b>\r\n\r\n";
decriptionHelper += "Cadastrar uma vacina: Uma vacina consiste em um nome e um identificador único.\r\n\r\n\r\n\r\n";

decriptionHelper += "<b>Person:</b>\r\n\r\n";
decriptionHelper += "Cadastrar uma pessoa: Uma pessoa consiste em um nome e um número de identificação único.\r\n\r\n";
decriptionHelper += "Remover uma pessoa: Uma pessoa pode ser removida do sistema, o que também implica na exclusão de seu cartão de vacinação e todos os registros associados.\r\n\r\n\r\n\r\n";

decriptionHelper += "<b>Cartão de Vacinação:</b>\r\n\r\n";
decriptionHelper += "Cadastrar uma vacinação: Para uma pessoa cadastrada, é possível registrar uma vacinação, fornecendo informações como o identificador da vacina e a dose aplicada (A dose deve ser validada pelo sistema).\r\n\r\n";
decriptionHelper += "Consultar o cartão de vacinação de uma pessoa: Permite visualizar todas as vacinas registradas no cartão de vacinação de uma pessoa, incluindo detalhes como o nome da vacina, data de aplicação e doses recebidas.\r\n\r\n";
decriptionHelper += "Excluir registro de vacinação: Permite excluir um registro de vacinação específico do cartão de vacinação de uma pessoa.\r\n\r\n\r\n\r\n";

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
