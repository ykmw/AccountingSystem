describe('Login Page Test', () => {
    it('Visits the login page', () => {
      cy.visit('/Identity/Account/Login')
      cy.get('.form-group').contains('Log in').should('have.css', 'background-color', 'rgb(236, 27, 36)')
    })
  })
  