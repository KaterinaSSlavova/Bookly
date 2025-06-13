namespace Bookly.ViewModels
{
    public class DeleteModalViewModel
    {
        public string IdName { get; set; }
        public int Id { get; set; }
        public string ConIdName {  get; set; }
        public int? ConnectionId { get; set; }
        public string Entity { get; set; }
        public string DisplayName { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }

        public DeleteModalViewModel(int id, string idName, string entity, string displayName, string controller, string actionName)
        {
            this.Id = id;
            this.IdName = idName;
            this.Entity = entity;
            this.DisplayName = displayName;
            this.Controller = controller;
            this.ActionName = actionName;
        }

        public DeleteModalViewModel(int id,string idName, int? connectionId, string conIdName, string entity, string displayName, string controller, string actionName)
        {
            this.Id = id;
            this.IdName = idName;
            this.ConnectionId = connectionId;
            this.ConIdName = conIdName;
            this.Entity = entity;
            this.DisplayName = displayName;
            this.Controller = controller;
            this.ActionName = actionName;
        }
    }
}
