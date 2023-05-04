using Newtonsoft.Json;

namespace PersonatorConsumer
{
  static class Program
  {
    static void Main(string[] args)
    {
      string baseServiceUrl = @"https://personator.melissadata.net/";
      string serviceEndpoint = @"v3/WEB/ContactVerify/doContactVerify"; //please see https://www.melissa.com/developer/personator for more endpoints
      string license = "";
      string fullname = "";
      string addressline1 = "";
      string city = "";
      string state = "";
      string postal = "";
      string country = "";
      string email = "";
      string phone = "";

      ParseArguments(ref license, ref fullname, ref addressline1, ref city, ref state, ref postal, ref country, ref email, ref phone, args);
      CallAPI(baseServiceUrl, serviceEndpoint, license, fullname, addressline1, city, state, postal, country, email , phone);
    }

    static void ParseArguments(ref string license, ref string fullname, ref string addressline1, ref string city, ref string state, ref string postal,
      ref string country, ref string email, ref string phone, string[] args)
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
        if (args[i].Equals("--fullname"))
        {
          if (args[i + 1] != null)
          {
            fullname = args[i + 1];
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
        if (args[i].Equals("--email"))
        {
          if (args[i + 1] != null)
          {
            email = args[i + 1];
          }
        }
        if (args[i].Equals("--phone"))
        {
          if (args[i + 1] != null)
          {
            phone = args[i + 1];
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
        try
        {
          Console.WriteLine(APICall.Substring(i, 70));
        }
        catch
        {
          Console.WriteLine(APICall.Substring(i, APICall.Length - i));
        }
      }

      Console.WriteLine("\nAPI Response:");
      Console.WriteLine(prettyResponse);
    }
    
    static void CallAPI(string baseServiceUrl, string serviceEndPoint, string license, string fullname, string addressline1, string city, string state, string postal, string country,
      string email, string phone)
    {
      Console.WriteLine("\n=============== WELCOME TO MELISSA PERSONATOR CONSUMER CLOUD API ===============\n");
      
      bool shouldContinueRunning = true;
      while (shouldContinueRunning)
      {
        string inputFullName = "";
        string inputAddressLine1 = "";
        string inputCity = "";
        string inputState = "";
        string inputPostal = "";
        string inputCountry = "";
        string inputEmail = "";
        string inputPhone = "";

        if (string.IsNullOrEmpty(fullname) && string.IsNullOrEmpty(addressline1) && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(state) && string.IsNullOrEmpty(postal)
          && string.IsNullOrEmpty(country) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(phone))
        {
          Console.WriteLine("\nFill in each value to see results");

          Console.Write("FullName: ");
          inputFullName = Console.ReadLine();

          Console.Write("AddressLine1: ");
          inputAddressLine1 = Console.ReadLine();

          Console.Write("City: ");
          inputCity = Console.ReadLine();

          Console.Write("State: ");
          inputState = Console.ReadLine();

          Console.Write("Postal: ");
          inputPostal = Console.ReadLine();

          Console.Write("Country: ");
          inputCountry = Console.ReadLine();

          Console.Write("Email: ");
          inputEmail = Console.ReadLine();

          Console.Write("Phone: ");
          inputPhone = Console.ReadLine();
        }
        else
        {
          inputFullName = fullname;
          inputAddressLine1 = addressline1;
          inputCity = city;
          inputState = state;
          inputPostal = postal;
          inputCountry = country;
          inputEmail = email;
          inputPhone = phone;
        }

        while (string.IsNullOrEmpty(inputFullName) || string.IsNullOrEmpty(inputAddressLine1) || string.IsNullOrEmpty(inputCity) || string.IsNullOrEmpty(inputState) || string.IsNullOrEmpty(inputPostal)
          || string.IsNullOrEmpty(inputCountry) || string.IsNullOrEmpty(inputEmail) || string.IsNullOrEmpty(inputPhone))
        {
          Console.WriteLine("\nFill in missing required parameter");

          if (string.IsNullOrEmpty(inputFullName))
          {
            Console.Write("FullName: ");
            inputFullName = Console.ReadLine();
          }

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
            Console.Write("Postal: ");
            inputPostal = Console.ReadLine();
          }

          if (string.IsNullOrEmpty(inputCountry))
          {
            Console.Write("Country: ");
            inputCountry = Console.ReadLine();
          }

          if (string.IsNullOrEmpty(inputEmail))
          {
            Console.Write("Email: ");
            inputEmail = Console.ReadLine();
          }

          if (string.IsNullOrEmpty(inputPhone))
          {
            Console.Write("Phone: ");
            inputPhone = Console.ReadLine();
          }
        }

        Dictionary<string, string> inputs = new Dictionary<string, string>()
        {
            { "format", "json"},
            { "full", inputFullName},
            { "a1", inputAddressLine1},
            { "city", inputCity},
            { "state", inputState},
            { "postal", inputPostal},
            { "ctry", inputCountry},
            { "email", inputEmail},
            { "phone", inputPhone}       
        };

        Console.WriteLine("\n===================================== INPUTS ===================================\n");
        Console.WriteLine($"\t   Base Service Url: {baseServiceUrl}");
        Console.WriteLine($"\t  Service End Point: {serviceEndPoint}");
        Console.WriteLine($"\t           FullName: {inputFullName}");
        Console.WriteLine($"\t     Address Line 1: {inputAddressLine1}");
        Console.WriteLine($"\t               City: {inputCity}");
        Console.WriteLine($"\t              State: {inputState}");
        Console.WriteLine($"\t        Postal Code: {inputPostal}");
        Console.WriteLine($"\t            Country: {inputCountry}");
        Console.WriteLine($"\t              Email: {inputEmail}");
        Console.WriteLine($"\t              Phone: {inputPhone}");

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
        if (!string.IsNullOrEmpty(fullname + addressline1 + city + state + postal + country + email + phone))
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