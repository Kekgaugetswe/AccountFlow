using System;
using System.Text.RegularExpressions;

namespace AccountFlow.Web.Domain.Validators;

public static class SaIdValidator
{

   // Validate the South African ID number
    public static bool IsValidSouthAfricanId(string idNumber)
    {
        // Step 1: Check if the ID number is exactly 13 digits long and contains no letters
        if (idNumber.Length != 13 || !Regex.IsMatch(idNumber, @"^\d{13}$"))
        {
            return false;
        }

        // Step 2: Validate the Luhn checksum
        if (!IsValidLuhn(idNumber))
        {
            return false;
        }

        // If Luhn validation passes, return true
        return true;
    }

    // Luhn algorithm to validate the checksum of the ID number
    private static bool IsValidLuhn(string number)
    {
        int checksum = int.Parse(number.Substring(number.Length - 1));  // Last digit is checksum
        int total = 0;

        // Traverse the ID number from right to left
        for (int i = number.Length - 2; i >= 0; i--)
        {
            int sum = 0;
            int digit = int.Parse(number.Substring(i, 1));

            // Double every second digit starting from the right
            if (i % 2 == number.Length % 2)
            {
                digit = digit * 2;
            }

            sum = digit / 10 + digit % 10;
            total += sum;
        }

        // Check if the checksum is valid
        return total % 10 != 0 ? 10 - total % 10 == checksum : checksum == 0;
    }

}
