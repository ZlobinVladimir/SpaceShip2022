namespace SpaceBattle.Lib;
using Hwdtech;

public class CreateTree : ICommand{
    private string file;
    public CreateTree(string file){
        this.file = file;
    }

    public void Execute(){
        var decision = IoC.Resolve<Dictionary<int, object>>("Create.Tree");
        try{
            using (StreamReader stream = File.OpenText(file)){
                string? line;
                while ((line = stream.ReadLine()) != null){
                    var fill = line.Split().Select(c => Convert.ToInt32(c)).ToList();

                    foreach (var i in fill){
                        decision.TryAdd(i, new Dictionary<int, object>());
                        decision = (Dictionary<int, object>)decision[i];
                    }
                }
            }
        }
        catch (FileNotFoundException i){
            throw new FileNotFoundException(i.ToString());
        }
        catch (Exception i){
            throw new Exception(i.ToString());
        }

    }
}
