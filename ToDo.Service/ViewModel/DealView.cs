namespace ToDo.ViewModel
{
    public class DealView
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public bool Checked {get;set; }
                        
        public List<DealView> Children { get; set; } = new List<DealView>();
    }
}
