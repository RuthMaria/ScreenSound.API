namespace ScreenSound.API.Requests;

public record MusicaRequestEdit(int Id, string nome, int anoDeLancamento): MusicaRequest(nome, Id, anoDeLancamento);
