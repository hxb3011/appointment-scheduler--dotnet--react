using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Infrastructure.Business
{
    internal sealed class MedicineImpl : BaseEntity, IMedicine
    {
        internal PrescriptionDetail _prdetail;
        internal Medicine _medicine;
        internal MedicineImpl(Medicine medicine, PrescriptionDetail prescriptionDetail = null)
        {
            _medicine = medicine ?? throw new ArgumentNullException(nameof(medicine));
            _prdetail = prescriptionDetail;
        }
        string IMedicine.Name { get => _medicine.Name; set => _medicine.Name = value; }
        // string IMedicine.Image { get => _medicine.Image; set => _medicine.Image = value; }
        string IMedicine.Unit { get => _medicine.Unit; set => _medicine.Unit = value; }
        string IMedicine.Description
        {
            get
            {
                if (_prdetail != null)
                    throw new InvalidOperationException("This medicine is only a template.");
                return _prdetail.Description;
            }
            set
            {
                if (_prdetail != null)
                    throw new InvalidOperationException("This medicine is only a template.");
                _prdetail.Description = value;
            }
        }

        protected override Task<bool> Create()
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> Delete()
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> Initilize()
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> Update()
        {
            throw new NotImplementedException();
        }
    }
}
