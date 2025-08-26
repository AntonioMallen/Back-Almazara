using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;
using Back_Almazara.ViewModel;

namespace Back_Almazara.Service.V1
{
    public interface INoticeService
    {

        public RepositoryResult<List<NoticeDTO>> Index(string user_id_i);
        public RepositoryResult<List<NoticeDTO>> IndexFavorites(string user_id_i);
        public RepositoryResult<NoticeDetailDTO> Detail(string notice_id_i);
        public RepositoryResult<NoticeDTO> Create(NoticeCreateDTO notice);
        public RepositoryResult<NoticeDetailViewModel> Edit(NoticeDetailViewModel noticeDetail);
        public RepositoryResult<NoticeDTO> Delete(string notice_id_i);
        public RepositoryResult<FavoriteDTO> FavoriteNotice(FavoriteViewModel favoriteInfo);
    }
}
