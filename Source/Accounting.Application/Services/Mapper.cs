using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Accounting.Application.Models;
using Accounting.Domain;
using IdentityServer4.Extensions;

namespace Accounting.Application.Services
{
    public static class Mapper
    {
        public static async Task<List<InvoiceDto>> Map(List<Invoice> invoices)
        {
            var invoiceDtos = await Task.Run(() => new List<InvoiceDto>());
            foreach (var invoice in invoices)
            {
                invoiceDtos.Add(Map(invoice));
            }
            return invoiceDtos;
        }
        public static async Task<List<CustomerDto>> Map(List<Customer> customers)
        {
            var customerDtos = await Task.Run(() => new List<CustomerDto>());
            foreach (var customer in customers)
            {
                customerDtos.Add(Map(customer));
            }
            return customerDtos;
        }
        public static List<LineItemDto> Map(List<LineItem> lineItems)
        {
            var lineItemDtos = new List<LineItemDto>();
            foreach (var lineItem in lineItems)
            {
                lineItemDtos.Add(Map(lineItem));
            }
            return lineItemDtos;
        }
        public static List<LineItem> Map(List<LineItemDto> lineItemDtos)
        {
            var lineItems = new List<LineItem>();
            foreach (var lineItemdto in lineItemDtos)
            {
                lineItems.Add(Map(lineItemdto));
            }
            return lineItems;
        }
        public static LineItemDto Map(LineItem lineitem)
        {
            return new LineItemDto()
            {
                Id = lineitem.Id,
                Name = lineitem.Name,
                Description = lineitem.Description,
                Quantity = lineitem.Quantity,
                Amount = lineitem.Amount,
                GST = lineitem.GST,
                Total = lineitem.Total,
                IsZeroRated = lineitem.IsZeroRated,
                InvoiceId = lineitem.InvoiceId
            };
        }
        public static LineItem Map(LineItemDto lineItemDto)
        {
            var lineItem = new LineItem
            {
                Id = lineItemDto.Id,
                Name = lineItemDto.Name,
                Description = lineItemDto.Description,
                Quantity = lineItemDto.Quantity,
                Amount = lineItemDto.Amount,
                GST = lineItemDto.GST,
                Total = lineItemDto.Total,
                IsZeroRated = lineItemDto.IsZeroRated,
                InvoiceId = lineItemDto.InvoiceId
            };
            return lineItem;
        }
        public static InvoiceDto Map(Invoice invoice)
        {
            return new InvoiceDto()
            {
                Id = invoice.Id,
                InvoiceRef = invoice.InvoiceRef,
                PurchaseOrder = invoice.PurchaseOrder,
                Date = invoice.Date,
                IsGSTExclusive = invoice.IsGSTExclusive,
                SubTotal = invoice.SubTotal,
                GST = invoice.GST,
                Discount = invoice.Discount,
                Total = invoice.Total,
                Status = invoice.Status,
                LineItem = Map(invoice.LineItems)
            };
        }
        public static Invoice Map(InvoiceDto invoiceDto)
        {
            var invoice = new Invoice
            {
                Id = invoiceDto.Id,
                InvoiceRef = invoiceDto.InvoiceRef,
                PurchaseOrder = invoiceDto.PurchaseOrder,
                Date = invoiceDto.Date,
                IsGSTExclusive = invoiceDto.IsGSTExclusive,
                SubTotal = invoiceDto.SubTotal,
                GST = invoiceDto.GST,
                Discount = invoiceDto.Discount,
                Total = invoiceDto.Total,
                Status = invoiceDto.Status,
                LineItems = Map(invoiceDto.LineItem)
            };
            return invoice;
        }
        public static CustomerDto Map(Customer customer)
        {
            return new CustomerDto(customer.Name)
            {
                ContactName = customer.ContactName,
                ContactEmail = customer.ContactEmail,
                IsGSTExempt = customer.IsGSTExempt,
                Address = Map(customer.Address),
                Phone = Map(customer.Phone),
                Id = customer.Id,
                InvoiceId = customer.InvoiceId,
            };
        }
        public static Customer Map(CustomerDto customerDto)
        {
            var customer = new Customer(customerDto.Name)
            {
                Id = customerDto.Id,
                IsGSTExempt = customerDto.IsGSTExempt,
                ContactName = customerDto.ContactName,
                ContactEmail = customerDto.ContactEmail,
                Address = Map(customerDto.Address),
                Phone = Map(customerDto.Phone),
                InvoiceId = customerDto.InvoiceId
            };
            return customer;
        }
        public static PhoneDto Map(Phone phone)
        {
            return new PhoneDto
            (
                phone.PhoneNumberPrefix,
                phone.PhoneNumber
            );
        }
        public static Phone Map(PhoneDto phone)
        {
            return new Phone(
                phone.PhoneNumberPrefix,
                phone.PhoneNumber);
        }
        public static AddressDto Map(Address address)
        {
            return new AddressDto()
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                PostCode = address.PostCode,
                Country = address.Country
            };
        }
        public static Address Map(AddressDto address)
        {
            return new Address(
                address.AddressLine1,
                address.AddressLine2,
                address.AddressLine3,
                address.PostCode,
                address.Country);
        }
        public static Organisation Map(OrganisationForRegistrationDto dto)
        {
            var org = new Organisation(
                dto.Name,
                dto.ShortCode,
                new Address(dto.AddressStreet1, dto.AddressStreet2, dto.AddressCityTown, dto.PostCode, dto.Country),
                new Phone(dto.PhonePrefix, dto.Phone));
            if (!string.IsNullOrEmpty(dto.GSTNumber))
            {
                org.GSTNumber = dto.GSTNumber;
            }
            return org;
        }
        public static OrganisationForRegistrationDto Map(Organisation organisation)
        {
            var dto = new OrganisationForRegistrationDto
            {
                Name = organisation.Name,
                ShortCode = organisation.ShortCode,
                GSTNumber = organisation.GSTNumber!,
                AddressStreet1 = organisation.Address.AddressLine1,
                AddressStreet2 = organisation.Address.AddressLine2,
                AddressCityTown = organisation.Address.AddressLine3,
                Country = organisation.Address.Country,
                PostCode = organisation.Address.PostCode,
                PhonePrefix = organisation.Phone.PhoneNumberPrefix,
                Phone = organisation.Phone.PhoneNumber
            };
            return dto;
        }
    }
}
