using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

Console.WriteLine("=== Week 1 : FHIR Get Exercise ===");
Console.WriteLine("Connecting to HAPI FHIR test Server...");
Console.WriteLine();

// Create a conenction to the public HAPI FHIR R4 Test Server
var client = new FhirClient("http://hapi.fhir.org/baseR4");

try
{
    // =========================
    // Section 1: Get Patient/Example information
    // =========================
    Console.WriteLine("Section 1: Get Patient/Example information");
    Console.WriteLine("Press any key to start...");
    Console.ReadLine();
    Console.Clear();

    var patient = await client.ReadAsync<Patient>("Patient/example");
    var gender = patient.Gender?.ToString() ?? "Unknown";
    var dob = !string.IsNullOrWhiteSpace(patient.BirthDate) ? patient.BirthDate : "Unknown";

    Console.WriteLine("Patient found:");
    Console.WriteLine($"  ID       : {patient.Id}");
    Console.WriteLine($"  Family   : {patient.Name[0].Family}");
    Console.WriteLine($"  Given    : {patient.Name[0].GivenElement[0].Value}");
    Console.WriteLine($"  Gender   : {gender}");
    Console.WriteLine($"  DOB      : {dob}");

    Console.WriteLine();
    Console.WriteLine("Raw identifiers on this patient:");

    foreach (var identifier in patient.Identifier)
    {
        Console.WriteLine($"  System   : {identifier.System}");
        Console.WriteLine($"  Value    : {identifier.Value}");
    }

    Console.WriteLine();
    Console.WriteLine("Press any key to continue to Section 2...");
    Console.ReadKey(true);
    Console.Clear();

    // =========================
    // Section 2: Get Raw JSON
    // =========================
    Console.WriteLine("Section 2: Get Raw JSON");
    Console.WriteLine();

    var serializer = new FhirJsonSerializer();
    var json = serializer.SerializeToString(patient);

    Console.WriteLine("Raw FHIR JSON:");
    Console.WriteLine(json);

    Console.WriteLine();
    Console.WriteLine("Press any key to continue to Section 3...");
    Console.ReadKey(true);
    Console.Clear();

    // =========================
    // Section 3: Search Patients with Family Name "Smith"
    // =========================
    Console.WriteLine("Section 3: Search Patients with Family Name \"Smith\"");
    Console.WriteLine();

    var searchResult = await client.SearchAsync<Patient>(
        new string[] { "family=Smith" }
    );

    Console.WriteLine($"Total Results: {searchResult.Total}");

    foreach (var entry in searchResult.Entry)
    {
        var p = (Patient)entry.Resource;

        Console.WriteLine(
            $"Found: {p.Name[0].Family}, {p.Name[0].GivenElement[0].Value} | DOB: {p.BirthDate}"
        );
    }
}
catch (FhirOperationException ex)
{
    Console.WriteLine($"FHIR Error: {ex.Message}");
    Console.WriteLine($"Status: {ex.Status}");
}
catch (Exception ex)
{
    Console.WriteLine($"General Error: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("Exercise complete.");
Console.WriteLine("Press any key to exit...");
Console.ReadKey(true);
