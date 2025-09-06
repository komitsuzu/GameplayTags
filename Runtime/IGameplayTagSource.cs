namespace BandoWare.GameplayTags
{
   internal interface IGameplayTagSource
   {
      public string Name { get; }

      public void RegisterTags(GameplayTagRegistrationContext context);
   }
}