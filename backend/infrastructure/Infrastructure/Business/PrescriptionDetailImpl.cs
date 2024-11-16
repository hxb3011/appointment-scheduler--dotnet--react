// #define DEMO


// #define DEMO

using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;

namespace AppointmentScheduler.Infrastructure.Business
{
    internal sealed class PrescriptionDetailImpl : BaseEntity, IPrescriptionDetail
    {
        internal PrescriptionDetail _prescriptionDetail;
        private IPrescription _prescription;
        private IMedicine _medicine;

        public PrescriptionDetailImpl(PrescriptionDetail prescriptionDetail, IPrescription prescription , IMedicine medicine)
        {
            _prescriptionDetail = prescriptionDetail?? throw new ArgumentNullException(nameof(prescriptionDetail));
            _prescription = prescription;
            _medicine = medicine;
        }

        IMedicine IPrescriptionDetail.Medicine { get => _medicine; set => _medicine = value; }
        IPrescription IPrescriptionDetail.Prescription { get => _prescription; }
        string IPrescriptionDetail.Description { get => _prescription.Description; set => _prescription.Description = value; }

       

        protected override Task<bool> Create()
        {
            _dbContext.Add(_prescriptionDetail);
            return Task.FromResult(true);
        }

        protected override Task<bool> Delete()
        {
            _dbContext.Remove(_prescriptionDetail);
            return Task.FromResult(true);
        }

        protected override Task<bool> Update()
        {
            _dbContext.Update(_prescriptionDetail);
            return Task.FromResult(true);
        }
    }
}