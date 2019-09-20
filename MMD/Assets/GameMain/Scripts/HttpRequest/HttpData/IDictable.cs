
namespace Penny
{
    public interface IDictable
    {
        string ToJson();
        byte[] ToJsonData();
        void fromDict(string responseJson);
    }

}
