var builder = WebApplication.CreateBuilder(args);

// 🔧 Agrega servicios necesarios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Política CORS específica para Vercel
builder.Services.AddCors(options =>
{
	options.AddPolicy("PermitirVercel", policy =>
	{
		policy.WithOrigins("https://app-proyecto-chi.vercel.app")
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
app.UseCors("PermitirVercel");

// ✅ Archivos estáticos y middlewares
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();
