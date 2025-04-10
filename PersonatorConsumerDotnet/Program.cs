using Newtonsoft.Json;
using System.Security.Cryptography;

namespace PersonatorConsumerDotnet
{
  static class Program
  {
    static void Main(string[] args)
    {
      string baseServiceUrl = @"https://personator.melissadata.net/";
      string serviceEndpoint = @"v3/WEB/ContactVerify/doContactVerify";
      string license = "";
      string addressline1 = "";
      string city = "";
      string state = "";
      string postal = "";
      string country = "";

      ParseArguments(ref license, ref addressline1, ref city, ref state, ref postal, ref country, args);
      CallAPI(baseServiceUrl, serviceEndpoint, license, addressline1, city, state, postal, country);
    }

    static void ParseArguments(ref string license, ref string addressline1, ref string city, ref string state, ref string postal,
      ref string country, string[] args)
    {
      for (int i = 0; i < args.Length; i++)
      {
        if (args[i].Equals("--license") || args[i].Equals("-l"))
        {
          if (args[i + 1] != null)
          {
            license = args[i + 1];
          }
        }
        if (args[i].Equals("--addressline1"))
        {
          if (args[i + 1] != null)
          {
            addressline1 = args[i + 1];
          }
        }
        if (args[i].Equals("--city"))
        {
          if (args[i + 1] != null)
          {
            city = args[i + 1];
          }
        }
        if (args[i].Equals("--state"))
        {
          if (args[i + 1] != null)
          {
            state = args[i + 1];
          }
        }
        if (args[i].Equals("--postal"))
        {
          if (args[i + 1] != null)
          {
            postal = args[i + 1];
          }
        }
        if (args[i].Equals("--country"))
        {
          if (args[i + 1] != null)
          {
            country = args[i + 1];
          }
        }
      }
    }

    public static async Task GetContents(string baseServiceUrl, string requestQuery)
    {
      HttpClient client = new HttpClient();
      client.BaseAddress = new Uri(baseServiceUrl);
      HttpResponseMessage response = await client.GetAsync(requestQuery);

      string text = await response.Content.ReadAsStringAsync();

      var obj = JsonConvert.DeserializeObject(text);
      var prettyResponse = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);

      // Print output
      Console.WriteLine("\n==================================== OUTPUT ====================================\n");

      Console.WriteLine("API Call: ");
      string APICall = Path.Combine(baseServiceUrl, requestQuery);
      for (int i = 0; i < APICall.Length; i += 70)
      {
        if (i + 70 < APICall.Length)
        {
          Console.WriteLine(APICall.Substring(i, 70));
        }
        else
        {
          Console.WriteLine(APICall.Substring(i, APICall.Length - i));
        }
      }

      Console.WriteLine("\nAPI Response:");
      Console.WriteLine(prettyResponse);
    }

    static void CallAPI(string baseServiceUrl, string serviceEndPoint, string license, string addressline1, string city, string state, string postal, string country)
    {
      Console.WriteLine("\n=============== WELCOME TO MELISSA PERSONATOR CONSUMER CLOUD API ===============\n");

      bool shouldContinueRunning = true;
      while (shouldContinueRunning)
      {
        string inputAddressLine1 = "";
        string inputCity = "";
        string inputState = "";
        string inputPostal = "";
        string inputCountry = "";


        if (string.IsNullOrEmpty(addressline1) && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(state) && string.IsNullOrEmpty(postal)
          && string.IsNullOrEmpty(country))
        {
          Console.WriteLine("\nFill in each value to see results");

          Console.Write("AddressLine1: ");
          inputAddressLine1 = Console.ReadLine();

          Console.Write("City: ");
          inputCity = Console.ReadLine();

          Console.Write("State: ");
          inputState = Console.ReadLine();

          Console.Write("PostalCode: ");
          inputPostal = Console.ReadLine();

          Console.Write("Country: ");
          inputCountry = Console.ReadLine();

        }
        else
        {
          inputAddressLine1 = addressline1;
          inputCity = city;
          inputState = state;
          inputPostal = postal;
          inputCountry = country;

        }

        while (string.IsNullOrEmpty(inputAddressLine1) || string.IsNullOrEmpty(inputCity) || string.IsNullOrEmpty(inputState) || string.IsNullOrEmpty(inputPostal)
          || string.IsNullOrEmpty(inputCountry))
        {
          Console.WriteLine("\nFill in missing required parameter");


          if (string.IsNullOrEmpty(inputAddressLine1))
          {
            Console.Write("AddressLine1: ");
            inputAddressLine1 = Console.ReadLine();
          }

          if (string.IsNullOrEmpty(inputCity))
          {
            Console.Write("City: ");
            inputCity = Console.ReadLine();
          }

          if (string.IsNullOrEmpty(inputState))
          {
            Console.Write("State: ");
            inputState = Console.ReadLine();
          }

          if (string.IsNullOrEmpty(inputPostal))
          {
            Console.Write("PostalCode: ");
            inputPostal = Console.ReadLine();
          }

          if (string.IsNullOrEmpty(inputCountry))
          {
            Console.Write("Country: ");
            inputCountry = Console.ReadLine();
          }
        }

        Dictionary<string, string> inputs = new Dictionary<string, string>()
        {
            { "format", "json"},
            { "cols", "GrpGeocode"},
            { "act", "Check"},
            { "a1", inputAddressLine1},
            { "city", inputCity},
            { "state", inputState},
            { "postal", inputPostal},
            { "ctry", inputCountry},
        };

        Console.WriteLine("\n==================================== INPUTS ====================================\n");
        Console.WriteLine($"\t   Base Service Url: {baseServiceUrl}");
        Console.WriteLine($"\t  Service End Point: {serviceEndPoint}");
        Console.WriteLine($"\t     Address Line 1: {inputAddressLine1}");
        Console.WriteLine($"\t               City: {inputCity}");
        Console.WriteLine($"\t              State: {inputState}");
        Console.WriteLine($"\t         PostalCode: {inputPostal}");
        Console.WriteLine($"\t            Country: {inputCountry}");

        // Create Service Call
        // Set the License String in the Request
        string RESTRequest = "";

        RESTRequest += @"&id=" + Uri.EscapeDataString(license);

        // Set the Input Parameters
        foreach (KeyValuePair<string, string> kvp in inputs)
          RESTRequest += @"&" + kvp.Key + "=" + Uri.EscapeDataString(kvp.Value);

        // Build the final REST String Query
        RESTRequest = serviceEndPoint + @"?" + RESTRequest;

        // Submit to the Web Service. 
        bool success = false;
        int retryCounter = 0;

        do
        {
          try //retry just in case of network failure
          {
            GetContents(baseServiceUrl, $"{RESTRequest}").Wait();
            Console.WriteLine();
            success = true;
          }
          catch (Exception ex)
          {
            retryCounter++;
            Console.WriteLine(ex.ToString());
            return;
          }
        } while ((success != true) && (retryCounter < 5));

        bool isValid = false;
        if (!string.IsNullOrEmpty(addressline1 + city + state + postal + country))
        {
          isValid = true;
          shouldContinueRunning = false;
        }

        while (!isValid)
        {
          Console.WriteLine("\nTest another record? (Y/N)");
          string testAnotherResponse = Console.ReadLine();

          if (!string.IsNullOrEmpty(testAnotherResponse))
          {
            testAnotherResponse = testAnotherResponse.ToLower();
            if (testAnotherResponse == "y")
            {
              isValid = true;
            }
            else if (testAnotherResponse == "n")
            {
              isValid = true;
              shouldContinueRunning = false;
            }
            else
            {
              Console.Write("Invalid Response, please respond 'Y' or 'N'");
            }
          }
        }
      }

      Console.WriteLine("\n===================== THANK YOU FOR USING MELISSA CLOUD API ====================\n");
    }
  }
}
