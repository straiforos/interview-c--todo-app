import { Languages } from 'lucide-react';
import { i18nService } from '@/services/i18n.service';
import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { useObservable } from '@/hooks/useObservable';
import { useLingui } from '@lingui/react/macro';

/**
 * A reusable language selector component that integrates with i18nService.
 */
export function LanguageSelect() {
  const { t } = useLingui();
  const currentLocale = useObservable(i18nService.locale$, 'en');

  const handleLocaleChange = (locale: string) => {
    i18nService.activate(locale);
  };

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" size="sm" className="gap-2">
          <Languages className="h-4 w-4" />
          <span className="uppercase">{currentLocale}</span>
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        <DropdownMenuItem onClick={() => handleLocaleChange('en')}>
          {t({ id: 'lang.english', message: 'English' })}
        </DropdownMenuItem>
        <DropdownMenuItem onClick={() => handleLocaleChange('es')}>
          {t({ id: 'lang.spanish', message: 'Spanish' })}
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
