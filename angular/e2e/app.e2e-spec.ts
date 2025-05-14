import { ProjectBlackTemplatePage } from './app.po';

describe('ProjectBlack App', function() {
  let page: ProjectBlackTemplatePage;

  beforeEach(() => {
    page = new ProjectBlackTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
