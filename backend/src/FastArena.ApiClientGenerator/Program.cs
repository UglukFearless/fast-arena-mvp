using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;

Console.WriteLine("Generation started.");

if (args.Length < 3)
    new ArgumentException("Expecting 3 arguments: URL, generatePath, language");

var url = args[0];
var generatePath = args[1];
var language = args[2];

if (language != "ts" && language != "sharp")
    new ArgumentException("Invalid language parameter; valid values are ts and sharp");

if (language == "ts")
    await GenerateTypeScriptClient(url, generatePath);
else
    await GenerateCSharpClient(url, generatePath);

Console.WriteLine("Generation finished.");
Console.ReadKey();

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