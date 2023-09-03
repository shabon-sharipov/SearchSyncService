using SearchSyncService.ElasticSearchRepository;
using SearchSyncService.MongoDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions<ElasticSearchOptions>("ElasticSearch");
builder.Services.AddSingleton<IStudentEventEsSyncConsumer, StudentEventEsSyncConsumer>();
builder.Services.AddSingleton<IStudentSyncService, StudentSyncService>();
builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
builder.Services.AddSingleton<IStudentRepositoryEs, StudentRepositoryEs>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Create an instance of the StudentEventEsSyncConsumer class
var studentEventEsSyncConsumer = app.Services.GetRequiredService<IStudentEventEsSyncConsumer>();
// Call the Consume method to start consuming events
await studentEventEsSyncConsumer.Consume();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();