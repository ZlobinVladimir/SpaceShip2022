using Hwdtech;

namespace SpaceBattle.Lib {
    public class GetDiffStrategy : IStrategy
    {
        public object Run(params object[] args)
        {
            var list1 = IoC.Resolve<List<int>>("Collision.GetList", (IUObject) args[0]);
            var list2 = IoC.Resolve<List<int>>("Collision.GetList", (IUObject) args[1]); 

            var dlist = list1.Zip(list2, (l1, l2) => l1 - l2);

            return dlist;
        }
        
    }
}