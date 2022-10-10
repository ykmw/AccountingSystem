export interface ICustomer {
    id: number,
    name: string,
    isGSTExempt: boolean,
    contactName: string,
    contactEmail: string,
    address: {
        addressLine1: string,
        addressLine2: string,
        addressLine3: string,
        country: string
        postCode: string
    },
    phone: {
        phoneNumberPrefix: number,
        phoneNumber: number
    },
    invoiceId: number
}