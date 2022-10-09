//вспомогательны класс, хранит количество типов ресурсов
public static class ResourceType
{
	public static readonly int Count = System.Enum.GetNames(typeof (RESOURCE_TYPE)).Length;
}
