namespace Bookly.ViewModels
{
    public class DeleteModalViewModel
    {
        public int Id { get; set; }
        public string Entity { get; set; }
        public string DisplayName { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }

        public DeleteModalViewModel(int id, string entity, string displayName, string controller, string actionName)
        {
             this.Id = id;
            this.Entity = entity;
            this.DisplayName = displayName;
            this.Controller = controller;
            this.ActionName = actionName;
        }
    }
}
