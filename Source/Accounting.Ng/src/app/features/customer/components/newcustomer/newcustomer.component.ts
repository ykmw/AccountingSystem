import { ChangeDetectorRef, Component, NgZone } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { Customer } from "../../classes/customer";
import { MyTel } from "../../models/my-test";
import { CustomerServiceService } from "../../service/customer-service.service";



@Component({
    selector:'app-new-customer',
    templateUrl:'./newcustomer.component.html',
    styleUrls:['newcustomer.component.scss']
})
export class NewCustomerComponent {

    title:string = "Register New Customer";
    IsGSTExempt:boolean = false;
    exemption!: boolean;

    gstcondition = [
        {"IsGSTExempt":"Exempted"},
        {"IsGSTExempt":"Not exempted"}
    ]


    onTouched: any = () => {};
    onChange: any = () => {};
    changeDetector: any;


    customerregform!: FormGroup;

    form: FormGroup = new FormGroup({
        Phone: new FormControl(new MyTel('','')) 
    });

    get boolValue(){

        if(this.customerregform.value.IsGSTExempt === "Exempted"){

            this.exemption = true;
        }else{

            this.exemption = false;
        }

        return this.exemption;
    }

    get value():Customer {

        const {
            value: {
               Name,
               IsGSTExempt,
               ContactName,
               ContactEmail,
               address,
               phone,
               InvoiceId
            }
        }=this.customerregform;

        return new Customer(Name, IsGSTExempt, ContactName, ContactEmail, address, phone,InvoiceId )
        //return this.customerregform.value;
    } 

    set value(value: Customer){
    
        this.customerregform.setValue({
            Name: this.customerregform.value.Name,
            IsGSTExemp: this.customerregform.value.IsGSTExempt,
            ContactName: this.customerregform.value.ContactName,
            ContactEmail: this.customerregform.value.ContactEmail,
            address: this.customerregform.value.address,
            phone: this.customerregform.value.phone,
            InvoiceId: this.customerregform.value.InvoiceId
        });
        this.onChange(value);
        this.onTouched();
    }

    emailPattern = "[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";

    constructor(private fb: FormBuilder, private ref: ChangeDetectorRef,
                private router: Router, private ngZone: NgZone, private crudService: CustomerServiceService ){

        this.customerregform = this.fb.group({

            Name: [
                null,
                [Validators.required]
            ],

            IsGSTExempt: [
                null,
                [Validators.required]
            ],
            ContactName: [
                null,
                [Validators.required]
            ],

            ContactEmail: [
                null,
                [Validators.required, Validators.pattern(this.emailPattern)]
            ],
            address: [
                null,
                [Validators.required]
            ],
            phone: [
                null,
                [Validators.required]
            ],
            InvoiceId: [
                null,
                [Validators.required]
            ]
        })
    }


    public checkError = (controlName: string, errorName: string) => {
        return this.customerregform.controls[controlName].hasError(errorName);
    }

     onSubmit(): any {
         this.crudService.addCustomer(new Customer(this.customerregform.value.Name, 
                    this.boolValue, this.customerregform.value.ContactName,
                    this.customerregform.value.ContactEmail, this.customerregform.value.address,
                 this.customerregform.value.phone,
                    +this.customerregform.value.InvoiceId))    
             .subscribe((data) => {
                console.log("Data:", JSON.stringify(data))
                this.customerregform.reset({});
                console.log('Data added successfully')
                this.ngZone.run(() => this.router.navigateByUrl('https://localhost:5001')) 
            }, (err) => {
                console.log("ERROR Thrown:",  err)
            })  
 


           /*  onSubmit():any {
                console.log("DATA", this.customerregform.value )
            }
     */

        }
}