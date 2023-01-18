using Hwdtech;

namespace SpaceBattle.Lib {
    public class CollisionCheck : ICommand
    {
        private readonly IUObject Object1, Object2;
        public CollisionCheck(IUObject Object1, IUObject Object2)
        {
            this.Object1 = Object1;
            this.Object2 = Object2;
        }
        
        public void Execute()
        {   
            var dlist = IoC.Resolve<IEnumerable<int>>("Collision.GetDiff", Object1, Object2);

            if (IoC.Resolve<bool>("Collision.CheckWithTree", dlist)) 
            {
                throw new Exception("Collision");
            }
        }
    }
}
