var builder = WebApplication.CreateBuilder(args);

// 🔧 Agrega servicios necesarios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
	options.AddPolicy("PermitirTodo", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});
builder.Services.AddAuthorization();

var app = builder.Build();

// 🧪 Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// ✅ Aplica CORS
app.UseCors("PermitirTodo");

// ✅ Habilita archivos estáticos (wwwroot incluido)
app.UseStaticFiles(); // Esto sirve wwwroot por defecto

// ✅ Autorización (si la usas)
app.UseAuthorization();

// ✅ Mapea los controladores
app.MapControllers();

app.Run();
