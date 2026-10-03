namespace RefactoringLab;

public interface IEmailSender
{
    void Send(string to, string body);
}
