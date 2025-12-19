using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Security;

namespace STGCLauncher
{
    public static class UnityPrefsRegistry
    {
        private const string REGISTRY_SOFTWARE_PATH = "Software\\";
        private const string HASH_SUFFIX = "_h";

        public static string HashKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            }

            uint hash = 5381u;

            for (int i = 0; i < key.Length; i++)
            {
                hash = (hash * 33) ^ key[i];
            }

            return $"{key}{HASH_SUFFIX}{hash}";
        }

        private static string GetRegistryPath(string company, string product)
        {
            return $"{REGISTRY_SOFTWARE_PATH}{company}\\{product}";
        }

        public static void SetInt(string company, string product, string key, int value)
        {
            ValidateParameters(company, product, key);

            string hashedKey = HashKey(key);
            string registryPath = GetRegistryPath(company, product);

            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
                {
                    using (RegistryKey subKey = baseKey.CreateSubKey(registryPath, writable: true))
                    {
                        subKey?.SetValue(hashedKey, value, RegistryValueKind.DWord);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new SecurityException("No permission to write to registry", ex);
            }
            catch (Exception ex)
            {
                if (!(ex is ArgumentException))
                {
                    Debug.WriteLine($"Error writing to registry: {ex.Message}");
                    throw;
                }
            }
        }

        public static int GetInt(string company, string product, string key, int defaultValue = 0)
        {
            ValidateParameters(company, product, key);

            string hashedKey = HashKey(key);
            string registryPath = GetRegistryPath(company, product);

            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
                {
                    using (RegistryKey subKey = baseKey.OpenSubKey(registryPath, writable: false))
                    {
                        if (subKey == null) return defaultValue;

                        object value = subKey.GetValue(hashedKey, null);

                        if (value == null) return defaultValue;
                        if (value is int intValue) return intValue;

                        else if (value is string stringValue)
                        {
                            if (int.TryParse(stringValue, out int parsedValue)) return parsedValue;
                        }

                        return defaultValue;
                    }
                }
            }
            catch (SecurityException)
            {
                return defaultValue;
            }
            catch (Exception ex)
            {
                if (!(ex is ArgumentException)) Debug.WriteLine($"Error reading from registry: {ex.Message}");

                return defaultValue;
            }
        }

        public static bool HasKey(string company, string product, string key)
        {
            ValidateParameters(company, product, key);

            string hashedKey = HashKey(key);
            string registryPath = GetRegistryPath(company, product);

            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
                {
                    using (RegistryKey subKey = baseKey.OpenSubKey(registryPath, writable: false))
                    {
                        return subKey?.GetValue(hashedKey, null) != null;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool DeleteKey(string company, string product, string key)
        {
            ValidateParameters(company, product, key);

            string hashedKey = HashKey(key);
            string registryPath = GetRegistryPath(company, product);

            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
                {
                    using (RegistryKey subKey = baseKey.OpenSubKey(registryPath, writable: true))
                    {
                        if (subKey != null && subKey.GetValue(hashedKey, null) != null)
                        {
                            subKey.DeleteValue(hashedKey);
                            return true;
                        }

                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool DeleteAll(string company, string product)
        {
            if (string.IsNullOrWhiteSpace(company))
                throw new ArgumentException("Company name cannot be empty", nameof(company));

            if (string.IsNullOrWhiteSpace(product))
                throw new ArgumentException("Product name cannot be empty", nameof(product));

            string registryPath = GetRegistryPath(company, product);

            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
                {
                    baseKey.DeleteSubKeyTree(registryPath, throwOnMissingSubKey: false);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private static void ValidateParameters(string company, string product, string key)
        {
            if (string.IsNullOrWhiteSpace(company))
                throw new ArgumentException("Company name cannot be empty", nameof(company));

            if (string.IsNullOrWhiteSpace(product))
                throw new ArgumentException("Product name cannot be empty", nameof(product));

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be empty", nameof(key));
        }

        public static void SetBool(string company, string product, string key, bool value)
        {
            SetInt(company, product, key, value ? 1 : 0);
        }

        public static bool GetBool(string company, string product, string key, bool defaultValue = false)
        {
            int intValue = GetInt(company, product, key, defaultValue ? 1 : 0);
            return intValue != 0;
        }

        public static void SetString(string company, string product, string key, string value)
        {
            ValidateParameters(company, product, key);

            string hashedKey = HashKey(key);
            string registryPath = GetRegistryPath(company, product);

            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
                {
                    using (RegistryKey subKey = baseKey.CreateSubKey(registryPath, writable: true))
                    {
                        subKey?.SetValue(hashedKey, value ?? string.Empty, RegistryValueKind.String);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error writing string to registry: {ex.Message}");
                throw;
            }
        }

        public static string GetString(string company, string product, string key, string defaultValue = "")
        {
            ValidateParameters(company, product, key);

            string hashedKey = HashKey(key);
            string registryPath = GetRegistryPath(company, product);

            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
                {
                    using (RegistryKey subKey = baseKey.OpenSubKey(registryPath, writable: false))
                    {
                        if (subKey == null) return defaultValue;

                        object value = subKey.GetValue(hashedKey, null);
                        return value?.ToString() ?? defaultValue;
                    }
                }
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}