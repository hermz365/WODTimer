namespace HermzApp.WODTimer.Shared.DataModel
{
    public class MicrosoftAdsUnitsData
    {        
        public static string ADUNITID
        {
            get
            {
#if DEV
                // Test value https://msdn.microsoft.com/windows/uwp/monetize/test-mode-values
                return "10865270"; 
#else
                return "";
#endif
            }
        }

        public static string APPLICATIONID
        {
            get
            {
#if DEV
                // Test value https://msdn.microsoft.com/windows/uwp/monetize/test-mode-values
                return "3f83fe91-d6be-434d-a0ae-7351c5a997f1";
#else
                return "";
#endif
            }
        }
    }
}
