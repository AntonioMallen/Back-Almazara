using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;

namespace Back_Almazara.Repository.V1
{
    public interface INoticesRepository
    {
        RepositoryResult<List<NoticeDTO>> Index(int user_id_i);
        RepositoryResult<NoticeDetailDTO> Detail(int notice_id_i);
        RepositoryResult<TNotice> Create(TNotice notice);
        RepositoryResult<TNoticesDetail> Edit(NoticeEditDTO noticeDetail);
        RepositoryResult<TNotice> Delete(int notice_id_i);
        RepositoryResult<TFavorite> FavoriteNotice(TFavorite favoriteInfo);

    }
}
