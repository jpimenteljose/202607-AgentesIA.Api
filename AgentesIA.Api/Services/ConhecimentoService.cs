using AgentesIA.Api.Services;
using System.Data.Common;

using Npgsql;
using Pgvector;

namespace AgentesIA.Api.Services
{
    public class ConhecimentoService
{
        private readonly NpgsqlDataSource _dataSource;

        private readonly EmbeddingService _embeddingService;

        public ConhecimentoService(NpgsqlDataSource dataSource, EmbeddingService embeddingService)
{
            _dataSource = dataSource;
            _embeddingService = embeddingService;
        }

        public async Task Cadastrar(ConhecimentoRequest request)
{
            var embedding =
            await _embeddingService
            .Gerar(request.Texto);

            await using var connection =
            await _dataSource.OpenConnectionAsync();

            const string sql = """
                    INSERT INTO conhecimentos
                    (
                    id,
                    texto,
                    origem,
                    tipo,
                    embedding
                    )
                    VALUES
                    (
                    @id,
                    @texto,
                    @origem,
                    @tipo,
                    @embedding
                    );
                    """;

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue(
            "id",
            Guid.NewGuid()
            );

            command.Parameters.AddWithValue(
            "texto",
            request.Texto
            );

            command.Parameters.AddWithValue(
            "origem",
            "api"
            );

            command.Parameters.AddWithValue(
            "tipo",
            "conhecimento"
            );

            command.Parameters.AddWithValue(
            "embedding",
            new Vector(embedding)
            );

            await command.ExecuteNonQueryAsync();
        }
    }
}

public record ConhecimentoRequest(string Texto);
