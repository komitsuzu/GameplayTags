using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BandoWare.GameplayTags
{
   internal class FileGameplayTagSource : IGameplayTagSource
   {
      public static readonly string DirectoryPath = Path.Combine(Application.dataPath, "..", "ProjectSettings", "GameplayTags");

      public string Name { get; }
      public bool IsReadOnly => false;

      private string m_FilePath;

      public FileGameplayTagSource(string filePath)
      {
         m_FilePath = filePath;
         Name = Path.GetFileName(filePath);
      }

      public static IEnumerable<IGameplayTagSource> GetAllFileSources()
      {
         if (!Directory.Exists(DirectoryPath))
            yield break;

         foreach (string filePath in Directory.EnumerateFiles(DirectoryPath, "*.json"))
            yield return new FileGameplayTagSource(filePath);
      }

      public void RegisterTags(GameplayTagRegistrationContext context)
      {
         try
         {
            string fileContent = File.ReadAllText(m_FilePath);
            JObject root = JObject.Parse(fileContent);
            RegisterTags(root, string.Empty, context);
         }
         catch (Exception ex)
         {
            Debug.LogError($"Failed to fetch tags from file '{m_FilePath}': {ex.Message}");
         }
      }

      private void RegisterTags(JObject obj, string baseTag, GameplayTagRegistrationContext context)
      {
         foreach (JProperty property in obj.Properties())
         {
            if (!GameplayTagUtility.IsNameValid(property.Name, out string nameErrorMessage))
            {
               Debug.LogError($"Invalid tag name \"{property.Name}\" from file {m_FilePath}: {nameErrorMessage}");
               continue;
            }

            JToken commentToken = property.Value["Comment"];
            string comment = commentToken?.ToString();

            string tagName = string.IsNullOrEmpty(baseTag) ? property.Name : $"{baseTag}.{property.Name}";

            context.RegisterTag(tagName, comment, GameplayTagFlags.None, this);
            JToken childrenTags = property.Value["Children"];

            if (childrenTags is JObject children)
               RegisterTags(children, tagName, context);
         }
      }
   }
}