using System.Buffers.Text;
using AutoMapper;
using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Repository.V1;
using Back_Almazara.Utility;
using Back_Almazara.ViewModel;

namespace Back_Almazara.Service.V1
{
    public class NoticeService : INoticeService
    {

        private readonly IMapper _mapper;
        private readonly INoticesRepository _noticeEntity;
        private readonly IHashUtility _hashUtility;
        public NoticeService(IMapper mapper, INoticesRepository noticeEntity, IHashUtility hashUtility)
        {
            _mapper = mapper;
            _noticeEntity = noticeEntity;
            _hashUtility = hashUtility;
        }

        public RepositoryResult<NoticeDTO> Create(NoticeCreateDTO notice)
        {
            var tNotice = _mapper.Map<TNotice>(notice);
            //tNotice.ImageNv = byteArray;
            return _noticeEntity.Create(tNotice)
                .MapTo<TNotice, NoticeDTO>(_mapper);
        }

        public RepositoryResult<NoticeDTO> Delete(string notice_id_i)
        {
            var noticeID = _hashUtility.FromBase36(notice_id_i);

            return _noticeEntity.Delete((int)noticeID)
                .MapTo<TNotice, NoticeDTO>(_mapper);
        }

        public RepositoryResult<NoticeDetailDTO> Detail(string notice_id_i)
        {
            var noticeID = _hashUtility.FromBase36(notice_id_i);

            return _noticeEntity.Detail((int)noticeID);
        }

        public RepositoryResult<NoticeDetailViewModel> Edit(NoticeDetailViewModel noticeDetail)
        {
            var noticeID = (int)_hashUtility.FromBase36(noticeDetail.IdI ?? "");

            var NoticeDTO = new NoticeEditDTO
            {
                IdI = noticeID
                , NameNv = noticeDetail.NameNv
                , DescriptionNv = noticeDetail.DescriptionNv
                , ImageNv = noticeDetail.ImageNv
                , TitleNv = noticeDetail.TitleNv
                , SubtitleNv = noticeDetail.SubtitleNv
                , ContentNv = noticeDetail.ContentNv
            };

            var resultDTO = _noticeEntity.Edit(NoticeDTO)
                .MapTo<TNoticesDetail, NoticeEditDTO>(_mapper);

            var resultDTOVL = resultDTO.MapTo<NoticeEditDTO, NoticeDetailViewModel>(_mapper);

            return resultDTOVL;
        }

        public RepositoryResult<List<NoticeDTO>> Index(string user_id_i)
        {
            var decryptedID = -1;
            if (user_id_i != "-1")
                decryptedID = (int)_hashUtility.FromBase36(user_id_i);
            RepositoryResult<List<NoticeDTO>> notices = _noticeEntity.Index(decryptedID);

            if (notices.Success) { 
                notices.Data = notices.Data
                    .Select(n => {
                        return new NoticeDTO
                        {
                            IdI = int.TryParse(n.IdI, out int id) ? _hashUtility.ToBase36(id) : n.IdI,
                            NameNv = n.NameNv,
                            DescriptionNv = n.DescriptionNv,
                            ImageNv = n.ImageNv,
                            favorite = n.favorite
                        };
                    })
                    .ToList();
            }

            return notices;
        }

        public RepositoryResult<List<NoticeDTO>> IndexFavorites(string user_id_i)
        {
            var userID = _hashUtility.FromBase36(user_id_i);

            var noticeResult= _noticeEntity.Index((int)userID);

            if (noticeResult.Success)
            {
                noticeResult.Data = noticeResult.Data
                    .Where(notice => notice.favorite == true)
                    .Select(n => {
                        return new NoticeDTO
                        {
                            IdI = int.TryParse(n.IdI, out int id) ? _hashUtility.ToBase36(id) : n.IdI,
                            NameNv = n.NameNv,
                            DescriptionNv = n.DescriptionNv,
                            ImageNv = n.ImageNv,
                            favorite = n.favorite
                        };
                    })
                    .ToList();
            }
            return noticeResult;
        }
        public RepositoryResult<FavoriteDTO> FavoriteNotice(FavoriteViewModel favoriteInfo)
        {
            var userID = (int)_hashUtility.FromBase36(favoriteInfo.user_id_i);
            var noticeID = (int)_hashUtility.FromBase36(favoriteInfo.notice_id_i);

            var favorite = new TFavorite()
            {
                UserIdI = userID,
                NoticeIdI = noticeID
            };

            return _noticeEntity.FavoriteNotice(favorite)
                .MapTo<TFavorite, FavoriteDTO>(_mapper);
        }
    }
}
