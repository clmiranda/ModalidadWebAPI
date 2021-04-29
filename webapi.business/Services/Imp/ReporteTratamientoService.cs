using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.ReporteTratamientos;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class ReporteTratamientoService: IReporteTratamientoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReporteTratamientoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public List<ReporteTratamiento> GetAll(int id) {
            var lista = _unitOfWork.ReporteTratamientoRepository.FindByCondition(x=>x.MascotaId==id).ToList();
            return lista;
        }
        public async Task<ReporteTratamiento> GetReporteTratamiento(int id)
        {
            var reporteTratamiento = await _unitOfWork.ReporteTratamientoRepository.GetById(id);
            return reporteTratamiento;
        }
        public async Task<bool> CreateReporteTratamiento(ReporteTratamientoForCreateDto reporteTratamientoDto)
        {
            var reporteTratamiento = _mapper.Map<ReporteTratamiento>(reporteTratamientoDto);
            _unitOfWork.ReporteTratamientoRepository.Insert(reporteTratamiento);
            return await _unitOfWork.SaveAll();

        }
        public async Task<bool> UpdateReporteTratamiento(ReporteTratamientoForUpdateDto reporteTratamientoDto)
        {
            var reporteTratamiento = _mapper.Map<ReporteTratamiento>(reporteTratamientoDto);
            _unitOfWork.ReporteTratamientoRepository.Update(reporteTratamiento);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> DeleteReporteTratamiento(ReporteTratamiento reporteTratamiento)
        {
            _unitOfWork.ReporteTratamientoRepository.Delete(reporteTratamiento);
            return await _unitOfWork.SaveAll();
        }
    }
}