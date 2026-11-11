using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;

Console.WriteLine("Generation started.");

if (args.Length < 2)
    throw new ArgumentException("Expecting at least 2 arguments: generatePath, language; optional: host, port");

var generatePath = args[0];
var language = args[1];
var host = args.Length > 2 ? args[2] : "localhost";
var port = args.Length > 3 ? args[3] : "80";
if (language != "ts" && language != "sharp")
    throw new ArgumentException("Invalid language parameter; valid values are ts and sharp");

var url = $"http://{host}:{port}/swagger/FastArenaAPI/swagger.json";
Console.WriteLine($"Source: {url}");

if (language == "ts")
    await GenerateTypeScriptClient(url, generatePath);
else
    await GenerateCSharpClient(url, generatePath);

Console.WriteLine("Generation finished.");

async static Task GenerateTypeScriptClient(string url, string generatePath) =>
    await GenerateClient(
            document: await OpenApiDocument.FromUrlAsync(url),
            generatePath: generatePath,
            generateCode: (OpenApiDocument document) =>
            {
                var settings = new TypeScriptClientGeneratorSettings
                {
                    TypeScriptGeneratorSettings =
                    {
                        TypeStyle = TypeScriptTypeStyle.Interface,
                        TypeScriptVersion = 3.5M,
                        DateTimeType = TypeScriptDateTimeType.String,
                        MarkOptionalProperties = false,
                    },
                };


                var generator = new TypeScriptClientGenerator(document, settings);
                var code = generator.GenerateFile();

                return code;
            }
        );

async static Task GenerateCSharpClient(string url, string generatePath) =>
    await GenerateClient(
            document: await OpenApiDocument.FromUrlAsync(url),
            generatePath: generatePath,
            generateCode: (OpenApiDocument document) =>
            {
                var settings = new CSharpClientGeneratorSettings
                {
                    UseBaseUrl = false
                };

                var generator = new CSharpClientGenerator(document, settings);
                var code = generator.GenerateFile();

                return code;
            }
        );

async static Task GenerateClient(
    OpenApiDocument document,
    string generatePath,
    Func<OpenApiDocument, string> generateCode)
{
    Console.WriteLine($"Generating {generatePath}...");

    var code = generateCode(document);

    await System.IO.File.WriteAllTextAsync(generatePath, code);
}