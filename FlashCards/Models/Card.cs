namespace FlashCards.Models
{
    public class Card
    {

        // ������������� ������
        public int Id { get; set; }
        // ����� (������� �����)
        public string Term { get; set; }
        // ���������� (��������)
        public string Definition { get; set; }
        // �������� (�� ���������������)
        public string Context { get; set; }

    }
}