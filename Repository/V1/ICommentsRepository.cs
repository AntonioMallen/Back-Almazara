using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;

namespace Back_Almazara.Repository.V1
{
    public interface ICommentsRepository
    {
        RepositoryResult<List<CommentDTO>> Index(int notice_id_i);
        RepositoryResult<TComment> Create(TComment comment);
        RepositoryResult<TComment> Edit(TComment comment);
        RepositoryResult<TComment> Delete(int comment_id_i);

    }
}
