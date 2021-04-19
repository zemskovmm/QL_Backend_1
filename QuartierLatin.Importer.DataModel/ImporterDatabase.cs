﻿using System;
using System.Collections.Generic;

namespace QuartierLatin.Importer.DataModel
{
    public class ImporterDatabase
    {
        public List<ImporterUniversity> Universities { get; set; } = new();
        public List<ImporterCity> Cities { get; set; } = new();
        public List<ImporterSpecialtyCategory> Specialties { get; set; } = new();
    }
}