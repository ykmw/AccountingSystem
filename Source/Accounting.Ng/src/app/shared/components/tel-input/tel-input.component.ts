import { FocusMonitor } from '@angular/cdk/a11y';
import { BooleanInput, coerceBooleanProperty } from '@angular/cdk/coercion';
import { Component, ElementRef, HostBinding, Inject, Input, OnDestroy, Optional, Self, ViewChild } from '@angular/core';
import { AbstractControl, ControlValueAccessor, FormBuilder, FormGroup, NgControl, Validators } from '@angular/forms';
import { MatFormField, MatFormFieldControl, MAT_FORM_FIELD } from '@angular/material/form-field';
import { Subject } from 'rxjs';
import { MyTel } from '../../../features/customer/models/my-test';

@Component({
  selector: 'app-tel-input',
  templateUrl: './tel-input.component.html',
  styleUrls: ['./tel-input.component.scss'],

  providers: [{
    provide: MatFormFieldControl,
    useExisting: TelInputComponent
  }],
})
export class TelInputComponent implements ControlValueAccessor, MatFormFieldControl<MyTel>, OnDestroy {

  static nextId = 0;

  @ViewChild('phoneNumberPrefix') phoneNumberPrefixInput!: HTMLInputElement;
  @ViewChild('phoneNumber') phoneNumberInput!: HTMLInputElement;


  parts: FormGroup;
  stateChanges = new Subject<void>();
  focused = false;
  touched = false;
  controlType = 'app-tel-input';
  
  @HostBinding('id')
  id = `app-tel-input-${TelInputComponent.nextId++}`;
  onChange = (_:any) => {};
  onTouched = () => {};
  
  get empty() {
      const {
          value: {phoneNumberPrefix, phoneNumber}
      }=this.parts;

      return !phoneNumberPrefix && !phoneNumber;
  }
  
  @HostBinding('class.tel-floating')
  get shouldLabelFloat() {
      return this.focused || !this.empty;
  }

  @Input() userAriaDescribedBy?:string 

  @Input() get placeholder():string {
      return this._placeholder;
  }

  set placeholder(value: string){
      this._placeholder = value;
      this.stateChanges.next();
  }


  private _placeholder!:string;

  @Input() get required(): boolean {
      return this._required;
  }

  set required(value: boolean){
      this._required = coerceBooleanProperty(value);
      this.stateChanges.next();
  }


  private _required = false;


  @Input() get disabled(): boolean {
      return this._disabled;
  }

  set disabled(value:boolean){
      this._disabled = coerceBooleanProperty(value);
      this._disabled ? this.parts.disable() : this.parts.enable();
      this.stateChanges.next();
  }

  private _disabled = false;

  @Input() get value(): MyTel | null {
      if(this.parts.valid){
          const {
              value: {phoneNumberPrefix, phoneNumber}
          }=this.parts;
          return new MyTel(phoneNumberPrefix, phoneNumber)
      }

      return null;
  }


  set value(phone: MyTel | null){
      const {phoneNumberPrefix, phoneNumber} = phone || new MyTel('','');
      this.parts.setValue({phoneNumberPrefix, phoneNumber});
      this.stateChanges.next();
  }

  get errorState(): boolean {
      return this.parts.invalid && this.touched;
  }

  constructor(
      formBuilder: FormBuilder,
      private _focusMonitor: FocusMonitor,
      private _elementRef: ElementRef<HTMLElement>,
      @Optional() @Inject(MAT_FORM_FIELD) public _formField: MatFormField,
      @Optional() @Self() public ngControl: NgControl
  ){
      this.parts = formBuilder.group({
          
          phoneNumberPrefix:[
              null,
              [Validators.required, Validators.minLength(2), Validators.maxLength(2), Validators.pattern("^[0-9]*$")]
          ],

          phoneNumber:[
              null,
              [Validators.required, Validators.minLength(7), Validators.maxLength(7), Validators.pattern("^[0-9]*$")]
          ]
      });

      if(this.ngControl != null){
          this.ngControl.valueAccessor = this;
      }
  }


  onFocusIn(event: FocusEvent){
      if(!this.focused){
          this.focused = true;
          this.stateChanges.next();
      }
  }

  onFocusOut(event: FocusEvent){
      if(!this._elementRef.nativeElement.contains(event.relatedTarget as Element)){
          this.touched = true;
          this.focused = false;
          this.onTouched();
          this.stateChanges.next();
      }
  }

  autoFocusNext(control: AbstractControl, nextElement?: HTMLInputElement | null):void{
      if(!control.errors && !!nextElement){
          this._focusMonitor.focusVia(nextElement,'program');
      }
  }

  autoFocusPrev(control: AbstractControl, prevElement: HTMLInputElement):void{
      if(control.value.length < 1){
          this._focusMonitor.focusVia(prevElement, 'program');
      }
  }

  setDescribedByIds(ids: string[]){
      const controlElement = this._elementRef.nativeElement
          .querySelector('tel-input-container')!;
          controlElement.setAttribute('aria-describedby', ids.join(''))
  }
  
  
  onContainerClick(event: MouseEvent){
      if(this.parts.controls.phoneNumber.valid){
          this._focusMonitor.focusVia(this.phoneNumberInput, 'program')
      }else if(this.parts.controls.phoneNumberPrefix.valid){
          this._focusMonitor.focusVia(this.phoneNumberInput, 'program')
      }else{
          this._focusMonitor.focusVia(this.phoneNumberPrefixInput, 'program')
      }
  }
  writeValue(phone: MyTel): void {
      this.value = phone;
  }
  registerOnChange(fn: any): void {
      this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
      this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean):void {
      this.disabled = isDisabled;
  }

  public checkError = (controlName:string, errorName:string) => {
      return this.parts.controls[controlName].hasError(errorName);
  }

  
  ngOnDestroy(): void {
      this.stateChanges.complete();
      this._focusMonitor.stopMonitoring(this._elementRef);
  }

  _handleInput(control: AbstractControl, nextElement?: HTMLInputElement):void{
      this.autoFocusNext(control, nextElement);
      this.onChange(this.value);
  }

 

  static ngAcceptInputType_disabled: BooleanInput;
  static ngAcceptInputType_required: BooleanInput;
  

}
