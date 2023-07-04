Казанцев Игорь

Данный иснтрумент можно использовать для создания и контроля очереди в arcade idle играх.

struct PositionQueue необходим для хранения позиций в очереди и заняты ли они

class DataQueue хранит в себе объект в очереди и номер (индекс) в очереди

interface IItemQueue Интерфейс который необходимо реализовать для очереди. Что бы очередь автоматически смещалась

Методы:

public bool HasQueuePlace() Возвращает true если есть свободное место в очереди

public DataQueue GetItem() Возвращает из очереди DataQueue

private void ReplaceQueue Смещает очередь

public (Vector3 position, int index) GetFreePositionQueue() Возвращает свободную позицию из очереди и помечает её как занятую

public void AddInQueue(IItemQueue item, int indexPlace) Добавляет элемент в очередь, для будущего изъятия

