using UnityEngine;
using UnityEngine.UI;

public static class Validation
{
    // Validate the input string and parse it to a float.
    // If the input is not a valid float, return the default value.
    // Clamp the parsed float value between the specified min and max values.
    public static float ValidateAndParseFloat(string input, float min, float max, float defaultValue)
    {
        // Try to parse the input string to a float.
        if (float.TryParse(input, out float value))
        {
            // Clamp the parsed value to the specified range and return it.
            return Mathf.Clamp(value, min, max);
        }
        // If parsing fails, return the default value.
        return defaultValue;
    }
}
