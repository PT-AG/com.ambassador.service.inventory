﻿using MlkPwgen;

namespace Com.Ambassador.Service.Inventory.Lib.Helpers
{
    public static class CodeGenerator
    {
        private const int LENGTH = 8;
        private const string ALLOWED_CHARACTER = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";

        public static string GenerateCode()
        {
            return PasswordGenerator.Generate(length: LENGTH, allowed: ALLOWED_CHARACTER);
        }
    }
}
